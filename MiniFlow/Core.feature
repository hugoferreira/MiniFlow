Scenario: Simple Transition
 Given I have a simple process
 When I initialize the process
 And I execute [times] times
 Then there are 1 executing processes
 And the current node is [node]
 And the process is [state]

 Examples:
 |times|node|  state|
 |    1|   2|running|
 |    2|   3| closed|

Scenario: Exclusive Gateway
 Given an exclusive gateway scenario
 When I initialize the process
 And I set the transition to [transition]
 And I execute [times] times
 Then the current node is [task]
 And there are 1 executing processes
 And the process is [state]

 Examples:
 |times| transition|task|  state|
 |    2|       true|   2|running|
 |    2|      false|   3|running|
 |    3|       true|   4| closed|
 |    3|      false|   4| closed|

Scenario: Parallel Execution
 Given a paralel gateway scenario
 When I initialize the process
 And I execute [times] times
 Then there are [count] executing processes
 And [hold] on hold
 And the current nodes are [nodes]
 And the process is [state]

 Examples:
 |times|count|hold|nodes|  state|
 |    2|    3|   1| 2, 3|running|
 |    4|    1|   0|    4| closed|

Scenario: Split Conditional Execution
 Given a split conditional scenario
 When I initialize the process
 And I set the transition to [state]
 And I execute 2 times
 Then there are [count] executing processes
 And the current nodes are [nodes]
 And 1 on hold
 And the process is running

 Examples:
 |state|count|nodes|
 | true|    3| 2, 3|
 |false|    2|    4|