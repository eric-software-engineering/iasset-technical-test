Feature: AppControllerTest
  We check the results coming from the GetWeather method

Scenario: Call the service GetWeather with success
	Given we create an AppController
	When we call the service with the country "Australia" and the city "Sydney"
	Then we get the weather of "Sydney"

Scenario: Call the service GetWeather without result
	Given we create an AppController
	When we call the service with the country "Australia" and the city "NotACity"
	Then we get an empty response

Scenario: Call the service GetWeather with exception
	Given we create an AppController with Exception throwing client
	When we call the service with the country "Australia" and the city "Sydney"
	Then we get an error