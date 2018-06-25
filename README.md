# Newsscraper App

## What

Newsscraper is a news site indexing application.
It provides an REST-like API for third parties to use.

## Why

Newsscraper was built as part of a test assignment.

## How to build and run

Developed and tested under Window 10 with Visual Studio Community 2017
v15.7.3.
Environment prerequisites:

* Install PostgreSQL 9.6. Specify `postgres` for password.
* Install IIS 7.5

# Directions for running

* Open newsscraper.sln, set `newsscraper` as a startup project.
* Issue the following command in Package Manager prompt to create the database:
    `Update-Database`
  If all is well (postgres is up and running, password is correct), the database
  will get created and deployed.
* Next, run the `newsscraper` project. It will crawl and index tengrinews.kz website.
* To have a look at the API, run newsscraper.Web project

# A note on API

`/api/posts?from=d1&to=d2` displays articles that were published between d1
and d2. 'YYYY-MM-DD' date format is expected.

`/api/search?text=foo` displays hits for `foo` in indexed content

`/api/topten` returns 10 most frequently occuring lexemes in indexed documents

