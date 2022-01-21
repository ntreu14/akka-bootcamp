open Akka.FSharp

let myActorSystem = 
    System.create "MyActorSystem" <| Configuration.load ()

let consoleWriterActor = 
    spawn myActorSystem "consoleWriterActor" <| actorOf Actors.consoleWriterActor

let validationActor =
    Actors.validationActor consoleWriterActor
    |> actorOf2
    |> spawn myActorSystem "validationActor"

let consoleReaderActor = 
    Actors.consoleReaderActor validationActor
    |> actorOf2
    |> spawn myActorSystem "conosleWriterActor"

consoleReaderActor <! Actors.Start
myActorSystem.WhenTerminated.Wait ()