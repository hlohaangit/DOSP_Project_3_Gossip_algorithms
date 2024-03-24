# [COP5615] DOSP Project 3

## Team members
- Harshit Lohaan (UFID: 7615-8695)
- Vashist Hegde (UFID: 8721-8376)
- Rahul Mora (UFID: 4577-9236)
- Sushmitha Kasireddy (UFID: 5336-8700)

## Steps to run the program
1. Download the project
2. Move to the project directory
3. Run the following command to build the project `dotnet build`
3. Run the following command: `donet run <numNodes> <topology> <algorithm>`

## Gossip algorithm
In our gossip protocol implementation, each node in the network has a counter and is aware of the network topology. Nodes initiate a scheduler to periodically send messages to randomly selected neighbors upon first message receipt. A node is considered converged after receiving the message at least once, and the entire system converges when all nodes have done so. Nodes continue transmitting messages until they have received them nine times, aiding in system-wide convergence while limiting network overhead. This implementation strikes a balance between efficient message dissemination across the network and managing resource utilization, with scalability and fault tolerance as key considerations.
##### Counter Mechanism
Each node tracks the number of times it has received a message, using this count to determine when to stop propagating the message.
##### Topology
The network's arrangement dictates how nodes connect and communicate, influencing the selection of neighbors for efficient message dissemination.

#### What is working?
All the topologies have successfully converged various number of nodes

#### The largest network we managed to deal with for each type of topology is as follows:

| Topology | Largest network size |
|----------|-----------------|
| Line          | 20000 (~12 mins)     |
| 2D            | 20000     |
| Full          | 20000     |
| Imperfect 3D  | 20000     |

## PushSum algorithm
The Push Sum algorithm calculates the average value across nodes in a network. Each node starts with a value pair (si, wi), where si is the node's initial value and wi is set to 1. The algorithm operates in rounds, where in each round, si and wi are updated based on the sum of messages received in the previous round. Nodes then randomly select a neighbor, split their si and wi values in half, and send one half to the selected neighbor and retain the other half. Convergence for a node is determined when the difference in its last 10 consecutive si/wi (S/W) values is less than 10^-10. Testing with different counts for convergence showed that a count of 3 led to quick convergence but low accuracy, while a count of 15 improved accuracy but slowed convergence. Thus, a count of 10 was chosen as a balance between accuracy and convergence speed.

#### What is working?
All the topologies have successfully converged various number of nodes

#### The largest network we managed to deal with for each type of topology is as follows:

| Topology | Largest network size |
|----------|-----------------|
| Line          | 5000 (~16mins)      |
| 2D            | 10000      |
| Imperfect 3D  | 10000      |
| Full          | 10000      |
