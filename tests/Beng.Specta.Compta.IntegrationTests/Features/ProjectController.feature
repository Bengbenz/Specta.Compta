Feature: Project Web Api
	Handle Project Operations (CRUD) from ProjectController api
	Link to a feature: [ProjectController](Beng.Specta.Compta/Controllers/ProjectController)

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
	
@post
Scenario: Save a new project
	Given I give a minimum information about a project.
	When I save this project by web api.
	Then The project is saved in DB and returned.