Feature: Project List
	Gets Project list from ProjectController api
	Link to a feature: [ProjectController](Beng.Specta.Compta/Controllers/ProjectController.List)

@list
Scenario: Gets empty list of projects
	Given No project was added yet.
	When I ask the projects list.
	Then The result should be empty.
	
Scenario: Gets not-empty list of projects
	Given Three projects are already seeded.
	When I ask the projects list.
	Then The result should contain three projects.
	And The first is Test1
	And The second is Test2
	And the last is Test3