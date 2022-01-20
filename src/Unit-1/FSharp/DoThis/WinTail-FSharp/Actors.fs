﻿module Actors 

open System
open Akka.Actor
open Akka.FSharp
open Messages

type Command = 
| Start
| Continue
| Message of string
| Exit

let (|Message|Exit|) (str:string) =
    match str.ToLower() with
    | "exit" -> Exit
    | _ -> Message(str)

let consoleReaderActor (consoleWriter: IActorRef) (mailbox: Actor<_>) message = 
    let (|EmptyMessage|MessageLengthIsEven|MessageLengthIsOdd|) (msg: string) =
        match msg.Length, msg.Length % 2 with
        | 0, _ -> EmptyMessage
        | _, 0 -> MessageLengthIsEven
        | _, _ -> MessageLengthIsOdd
   
    let doPrintInstructions () =
        Console.WriteLine "Write whatever you want into the console!"
        Console.WriteLine "Some entries will pass validation, and some won't...\n\n"
        Console.WriteLine "Type 'exit' to quit this application at any time.\n"
   
    let getAndValidateInput () =
        let line = Console.ReadLine ()
        match line with
        | Exit -> mailbox.Context.System.Terminate() |> ignore
        | Message input -> 
            match input with 
            | EmptyMessage -> 
                mailbox.Self <! InputError ("No input received", ErrorType.Null)

            | MessageLengthIsEven ->
               consoleWriter <! InputSuccess ("Thank you! The message was valid.")
               mailbox.Self <! Continue

            | _ ->
               mailbox.Self <! InputError ("The message is invalid (odd number of characters)!", ErrorType.Validation)
   
    match box message with
    | :? Command as command ->
       match command with
       | Start -> doPrintInstructions ()
       | _ -> ()

    | :? InputResult as inputResult ->
       match inputResult with
       | InputError _ as error -> consoleWriter <! error
       | _ -> ()

    | _ -> ()  

    getAndValidateInput ()

let consoleWriterActor message = 
    let printInColor color message =
        Console.ForegroundColor <- color
        Console.WriteLine (message.ToString ())
        Console.ResetColor ()

    match box message with
    | :? InputResult as inputResult ->
        match inputResult with
        | InputError (reason, _) -> printInColor ConsoleColor.Red reason
        | InputSuccess reason -> printInColor ConsoleColor.Green reason

    | _ -> 
        message.ToString () |> printInColor ConsoleColor.Yellow 