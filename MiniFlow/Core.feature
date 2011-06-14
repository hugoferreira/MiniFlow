Scenario: Simple Transition
 Given I have a simple process
 When I initialize the process
 And I execute [times] times
 Then there are 1 executing processes
 And the current node is [node]
 And it is [state] that the process is finished

 Examples:
 |times|node|state|
 |    1|   2|false|
 |    2|   3| true|

Scenario: Exclusive Gateway
 Given an exclusive gateway scenario
 When I initialize the process
 And I execute the current task
 And I set the transition to [state]
 And I execute again
 Then the current node is [task]
 And the process is not finished

 Examples:
 |state|task|
 | true|   2|
 |false|   3|

Scenario: Parallel Execution
 Given a paralel gateway scenario
 When I initialize the process
 And I execute [times] times
 Then there are [count] executing processes
 And [hold] on hold
 And the current nodes are [nodes]
 And it is [state] that the process is finished

 Examples:
 |times|count|hold|nodes|state|
 |    2|    3|   1| 2, 3|false|
 |    4|    1|   0|    4| true|

Scenario: Split Conditional Execution
 Given a split conditional scenario
 When I initialize the process
 And I execute the current task
 And I set the transition to [state]
 And I execute again
 Then there are [count] executing processes
 And the current nodes are [nodes]
 And 1 on hold
 And the process is not finished

 Examples:
 |state|count|nodes|
 | true|    3| 2, 3|
 |false|    2|    4|