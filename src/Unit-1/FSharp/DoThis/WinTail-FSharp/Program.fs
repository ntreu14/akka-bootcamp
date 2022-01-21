open Akka.FSharp

let myActorSystem = System.create "MyActorSystem" <| Configuration.load ()

let consoleWriterActor = 
    spawn myActorSystem "consoleWriterActor" <| actorOf Actors.consoleWriterActor

let consoleReaderActor = 
    Actors.consoleReaderActor consoleWriterActor
    |> actorOf2
    |> spawn myActorSystem "conosleWriterActor"

consoleReaderActor <! Actors.Start
myActorSystem.WhenTerminated.Wait ()