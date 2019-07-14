# WheelMovies
A movie database API application.

#### Below is the tech stack used
* Asp.Net Core 2.2
* Sql Server Express 2016

#### Below is the database schema used. Database script is [here](https://github.com/DeepakChoudhari/WheelMovies/blob/master/WheelMoviesDbScript.sql)
![WheelMovies databse schema](https://github.com/DeepakChoudhari/WheelMovies/blob/master/db_schema.png)

#### What's missing? (Improvement Opportunities)
* Security - authentication and authorization of apis' has been omitted
* API Health checks
* Caching - Caching can be leveraged to cache the responses
* Add more unit and integration tests to cover all the scenarios
* API Versioning (via headers)
* Implement HATEOAS - Hypermedia As Transfer Engine Of Application State 