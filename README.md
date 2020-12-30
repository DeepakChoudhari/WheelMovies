# WheelMovies
A movie database Web API.

#### Below is the tech stack used
* Asp.Net Core 2.2
* Sql Server Express 2016

#### Below is the database schema used. Database script is [here](https://github.com/DeepakChoudhari/WheelMovies/blob/master/WheelMoviesDbScript.sql)
![WheelMovies databse schema](https://github.com/DeepakChoudhari/WheelMovies/blob/master/db_schema.png)

#### What can be added next? (Improvement Opportunities)
* Add more unit and integration tests to cover all the scenarios
* Concurrency checks while saving data
* Security - authentication and authorization of apis' has been omitted
* API Health checks
* Caching - Caching can be leveraged to cache the responses
* API Versioning (via headers)
* Implement HATEOAS - Hypermedia As Transfer Engine Of Application State