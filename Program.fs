open System
open Deedle
open Akka.FSharp


let roundNodes numNodes topology =
    match topology with
    | "2d"
    // | "imperfect2d" -> Math.Pow (Math.Round (sqrt (float numNodes)), 2.0) |> int
    // | "3d"
    | "imperfect3d" -> Math.Pow (Math.Round ((float numNodes) ** (1.0 / 3.0)), 3.0)  |> int
    | _ -> numNodes

let pickRandom (l: List<_>) =
    let r = Random()
    l.[r.Next(l.Length)]

let getRandomNeighborID (topologyMap: Map<_, _>) nodeID =
    let (neighborList: List<_>) = (topologyMap.TryFind nodeID).Value
    let random = Random()
    neighborList.[random.Next(neighborList.Length)]

let buildLineTopology numNodes =
    let mutable map = Map.empty
    [ 1 .. numNodes ]
    |> List.map (fun nodeID ->
        let listNeighbors = List.filter (fun y -> (y = nodeID + 1 || y = nodeID - 1)) [ 1 .. numNodes ]
        map <- map.Add(nodeID, listNeighbors))
    |> ignore
    map

let gridNeighbors2D nodeID numNodes =
    let mutable map = Map.empty
    let lenSide = sqrt (float numNodes) |> int
    [ 1 .. numNodes ]
    |> List.filter (fun y ->
        if (nodeID % lenSide = 0) then (y = nodeID - 1 || y = nodeID - lenSide || y = nodeID + lenSide)
        elif (nodeID % lenSide = 1) then (y = nodeID + 1 || y = nodeID - lenSide || y = nodeID + lenSide)
        else (y = nodeID - 1 || y = nodeID + 1 || y = nodeID - lenSide || y = nodeID + lenSide))
let build2DTopology numNodes =
    let mutable map = Map.empty
    [ 1 .. numNodes ]
    |> List.map (fun nodeID ->
        let listNeighbors = gridNeighbors2D nodeID numNodes
        map <- map.Add(nodeID, listNeighbors))
    |> ignore
    map

// let buildImperfect2DTopology numNodes =
//     let mutable map = Map.empty
//     [ 1 .. numNodes ]
//     |> List.map (fun nodeID ->
//         let mutable listNeighbors = gridNeighbors2D nodeID numNodes
//         let random =
//             [ 1 .. numNodes ]
//             |> List.filter (fun m -> m <> nodeID && not (listNeighbors |> List.contains m))
//             |> pickRandom
//         let listNeighbors = random :: listNeighbors
//         map <- map.Add(nodeID, listNeighbors))
//     |> ignore
//     map

let gridNeighbors3D nodeID numNodes =
    let lenSide = Math.Round(Math.Pow((float numNodes), (1.0 / 3.0))) |> int
    [ 1 .. numNodes ]
    |> List.filter (fun y ->
        if (nodeID % lenSide = 0) then
            if (nodeID % (int (float (lenSide) ** 2.0)) = 0) then
                (y = nodeID - 1 || y = nodeID - lenSide || y = nodeID - int ((float (lenSide) ** 2.0)) || y = nodeID + int ((float (lenSide) ** 2.0)))
            elif (nodeID % (int (float (lenSide) ** 2.0)) = lenSide) then
                (y = nodeID - 1 || y = nodeID + lenSide || y = nodeID - int ((float (lenSide) ** 2.0)) || y = nodeID + int ((float (lenSide) ** 2.0)))
            else
                (y = nodeID - 1 || y = nodeID - lenSide || y = nodeID + lenSide || y = nodeID - int ((float (lenSide) ** 2.0)) || y = nodeID + int ((float (lenSide) ** 2.0)))        
        elif (nodeID % lenSide = 1) then
            if (nodeID % (int (float (lenSide) ** 2.0)) = 1) then
                (y = nodeID + 1 || y = nodeID + lenSide || y = nodeID - int ((float (lenSide) ** 2.0)) || y = nodeID + int ((float (lenSide) ** 2.0)))
            elif (nodeID % (int (float (lenSide) ** 2.0)) = int (float (lenSide) ** 2.0) - lenSide + 1 ) then
                (y = nodeID + 1 || y = nodeID - lenSide || y = nodeID - int ((float (lenSide) ** 2.0)) || y = nodeID + int ((float (lenSide) ** 2.0)))
            else
                (y = nodeID + 1 || y = nodeID - lenSide || y = nodeID + lenSide || y = nodeID - int ((float (lenSide) ** 2.0)) || y = nodeID + int ((float (lenSide) ** 2.0)))
        elif (nodeID % (int (float (lenSide) ** 2.0)) > 1) && (nodeID % (int (float (lenSide) ** 2.0)) < lenSide) then
            (y = nodeID - 1 || y = nodeID + 1 || y = nodeID + lenSide || y = nodeID - int ((float (lenSide) ** 2.0)) || y = nodeID + int ((float (lenSide) ** 2.0)))
        elif (nodeID % (int (float (lenSide) ** 2.0)) > int (float (lenSide) ** 2.0) - lenSide + 1) && (nodeID % (int (float (lenSide) ** 2.0)) < (int (float (lenSide) ** 2.0))) then
            (y = nodeID - 1 || y = nodeID + 1 || y = nodeID - lenSide || y = nodeID - int ((float (lenSide) ** 2.0)) || y = nodeID + int ((float (lenSide) ** 2.0)))
        else
            (y = nodeID - 1 || y = nodeID + 1 || y = nodeID - lenSide || y = nodeID + lenSide || y = nodeID - int ((float (lenSide) ** 2.0)) || y = nodeID + int ((float (lenSide) ** 2.0))))

// let build3DTopology numNodes =
//     let mutable map = Map.empty
//     [ 1 .. numNodes ]
//     |> List.map (fun nodeID ->
//         let listNeighbors = gridNeighbors3D nodeID numNodes
//         map <- map.Add(nodeID, listNeighbors))
//     |> ignore
//     map

let buildImperfect3DTopology numNodes =
    let mutable map = Map.empty
    [ 1 .. numNodes ]
    |> List.map (fun nodeID ->
        let mutable listNeighbors = gridNeighbors3D nodeID numNodes
        let random =
            [ 1 .. numNodes ]
            |> List.filter (fun m -> m <> nodeID && not (listNeighbors |> List.contains m))
            |> pickRandom
        let listNeighbors = random :: listNeighbors
        map <- map.Add(nodeID, listNeighbors))
    |> ignore
    map

let buildFullTopology numNodes =
    let mutable map = Map.empty
    [ 1 .. numNodes ]
    |> List.map (fun nodeID ->
        let listNeighbors = List.filter (fun y -> nodeID <> y) [ 1 .. numNodes ]
        map <- map.Add(nodeID, listNeighbors))
    |> ignore
    map

let buildTopology numNodes topology =
    let mutable map = Map.empty
    match topology with
    | "line" -> buildLineTopology numNodes
    | "2d" -> build2DTopology numNodes
    // | "imperfect2d" -> buildImperfect2DTopology numNodes
    // | "3d" -> build3DTopology numNodes
    | "imperfect3d" -> buildImperfect3DTopology numNodes
    | "full" -> buildFullTopology numNodes


type CounterMessage =
    | GossipNodeConverge
    | PushSumNodeConverge of int * float

type Result = { NumberOfNodesConverged: int; TimeElapsed: int64; }

let counter initialCount numNodes (stopWatch: Diagnostics.Stopwatch) (mailbox: Actor<'a>) =
    let rec loop count (dataframeList: Result list) =
        actor {
            let! message = mailbox.Receive()
            match message with
            | GossipNodeConverge ->
                let newRecord = { NumberOfNodesConverged = count + 1; TimeElapsed = stopWatch.ElapsedMilliseconds; }
                if (count + 1 = numNodes) then
                    stopWatch.Stop()
                    printfn "[INFO] Gossip Algorithm has converged in %d ms" stopWatch.ElapsedMilliseconds
                    let dataframe = Frame.ofRecords dataframeList
                    mailbox.Context.System.Terminate() |> ignore
                return! loop (count + 1) (List.append dataframeList [newRecord])
            | PushSumNodeConverge (nodeID, avg) ->
                // printfn "[INFO] Node %d has been converged s/w=%f)" nodeID avg
                let newRecord = { NumberOfNodesConverged = count + 1; TimeElapsed = stopWatch.ElapsedMilliseconds }
                if (count + 1 = numNodes) then
                    stopWatch.Stop()
                    printfn "[INFO] Push Sum Algorithm has converged in %d ms" stopWatch.ElapsedMilliseconds
                    let dataframe = Frame.ofRecords dataframeList
                    mailbox.Context.System.Terminate() |> ignore
                return! loop (count + 1) (List.append dataframeList [newRecord])
        }
    loop initialCount []


let gossip maxCount (topologyMap: Map<_, _>) nodeID counterRef (mailbox: Actor<_>) = 
    let rec loop (count: int) = actor {
        let! message = mailbox.Receive ()
        match message with
        | "heardRumor" ->
            if count = 0 then
                mailbox.Context.System.Scheduler.ScheduleTellOnce(
                    TimeSpan.FromMilliseconds(25.0),
                    mailbox.Self,
                    "spreadRumor"
                )
                counterRef <! GossipNodeConverge
                return! loop (count + 1)
            else
                return! loop (count + 1)
        | "spreadRumor" ->
            if count >= maxCount then
                return! loop count
            else
                let neighborID = getRandomNeighborID topologyMap nodeID
                let neighborPath = @"akka://my-system/user/worker" + string neighborID
                let neighborRef = mailbox.Context.ActorSelection(neighborPath)
                neighborRef <! "heardRumor"
                mailbox.Context.System.Scheduler.ScheduleTellOnce(
                    TimeSpan.FromMilliseconds(25.0),
                    mailbox.Self,
                    "spreadRumor"
                )
                return! loop count
        | _ ->
            printfn "[INFO] Node %d has received unhandled message" nodeID
            return! loop count
    }
    loop 0
 
type PushSumMessage =
    | Initialize
    | Message of float * float
    | Round

let pushSum (topologyMap: Map<_, _>) nodeID counterRef (mailbox: Actor<_>) = 
    let rec loop sNode wNode sSum wSum count isTransmitting = actor {
        if isTransmitting then
            let! message = mailbox.Receive ()
            match message with
            | Initialize ->
                mailbox.Self <! Message (float nodeID, 1.0)
                mailbox.Context.System.Scheduler.ScheduleTellRepeatedly (
                    TimeSpan.FromMilliseconds(0.0),
                    TimeSpan.FromMilliseconds(25.0),
                    mailbox.Self,
                    Round
                )
                return! loop (float nodeID) 1.0 0.0 0.0 0 isTransmitting
            | Message (s, w) ->
                return! loop sNode wNode (sSum + s) (wSum + w) count isTransmitting
            | Round ->
                let neighborID = getRandomNeighborID topologyMap nodeID
                let neighborPath = @"akka://my-system/user/worker" + string neighborID
                let neighborRef = mailbox.Context.ActorSelection(neighborPath)
                mailbox.Self <! Message (sSum / 2.0, wSum / 2.0)
                neighborRef <! Message (sSum / 2.0, wSum / 2.0)
                if(abs ((sSum / wSum) - (sNode / wNode)) < 1.0e-10) then
                    let newCount = count + 1
                    if newCount = 10 then
                        counterRef <! PushSumNodeConverge (nodeID, sSum / wSum)
                        return! loop sSum wSum 0.0 0.0 newCount false
                    else
                        return! loop (sSum / 2.0) (wSum / 2.0) 0.0 0.0 newCount isTransmitting 
                else
                    return! loop (sSum / 2.0) (wSum / 2.0) 0.0 0.0 0 isTransmitting
    }
    loop (float nodeID) 1.0 0.0 0.0 0 true


[<EntryPoint>]
let main argv =
    let system = System.create "my-system" (Configuration.load())

    let maxCount = 10
    
    let topology = argv.[1]
    let numNodes = roundNodes (int argv.[0]) topology
    let algorithm = argv.[2]
    
    let topologyMap = buildTopology numNodes topology

    let stopWatch = Diagnostics.Stopwatch()

    let counterRef = spawn system "counter" (counter 0 numNodes stopWatch)

    match algorithm with
    | "gossip" ->
        let workerRef =
            [ 1 .. numNodes ]
            |> List.map (fun nodeID ->
                let name = "worker" + string nodeID
                spawn system name (gossip maxCount topologyMap nodeID counterRef))
            |> pickRandom
        stopWatch.Start()
        workerRef <! "heardRumor"

    | "push-sum" ->
        let workerRef =
            [ 1 .. numNodes ]
            |> List.map (fun nodeID ->
                let name = "worker" + string nodeID
                (spawn system name (pushSum topologyMap nodeID counterRef)))
        stopWatch.Start()
        workerRef |> List.iter (fun item -> item <! Initialize)


    system.WhenTerminated.Wait()
    0
