<!-- PROJECT SHIELDS -->



## About The Project :zap:

BlazorHero is a Clean Architecture Solution Template for Blazor Webassembly 6.0 built with MudBlazor Components.

## Complete Overview - Youtube Video 🆕 📈

So, here is an in-depth video that takes you through the BlazorHero Project! Do Like & Subscribe to my Youtube channel! It would be great if you could leave behind your valuable feedback in the comments section of the Video. This helps me reach a much wider audience with time :)

<a href="https://www.youtube.com/watch?v=j7BxKN7HYjk&lc=Ugy1SjDcmP0pboRxJe94AaABAg">Watch it here!</a>

[![Blazor Hero - Clean Architecture Solution Template for Blazor WebAssembly](http://img.youtube.com/vi/j7BxKN7HYjk/0.jpg)](http://www.youtube.com/watch?v=j7BxKN7HYjk "Blazor Hero - Clean Architecture Solution Template for Blazor WebAssembly")

### Tech Stack :muscle:

- Blazor WebAssembly 6.0 - ASP.NET Core Hosted Model
- [Entity Framework Core 6.0](https://docs.microsoft.com/en-us/ef/core/)

# BlazorHero v2.2

- UI Improvements
- Docker Support
- Better Permissions Management
- Code Cleanups
- RTL Support
- Minor Bug Fixes
- Better Project Structure

# What to Expect in BlazorHero 3.0?

- Modular Architecture
- Cleaner Separation Of Code
- Dedicated Documentation Website - [Here](https://blazorhero.github.io/docs/)
- Tutorials to add new entities, controllers
- UI Updates
- Support for PostgreSQL / MySQL - Easy DB Switching
- Theme Manager Integration to change UI Color Palletes / Fonts on the go.
- You can suggest your requirements as well!

# Down the Roadmap

 - Migration to .NET 6
 - Multi Tenancy
 - Better Localization - JSON

# Getting Started 🦸

> **Important**
If you are already using Blazor Hero v1.x, make sure that you drop your existing database and re-update your database using the CLI as there are a couple of new migrations added that might clash with your existing schema. Also, install the latest version of BlazorHero.

The easiest way to get started with Blazor Hero is to install the [NuGet package](https://www.nuget.org/packages/BlazorHero.CleanArchitecture/) and run `dotnet new BlazorHero.CleanArchitecture`:

1. Install the latest [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Install the latest DOTNET & EF CLI Tools by using this command `dotnet tool install --global dotnet-ef` 
3. Install the latest version of Visual Studio IDE 2019 (v16.8 and above) 🚀
4. Open up Command Prompt and run `dotnet new --install BlazorHero.CleanArchitecture` to install the project template
5. Create a folder for your solution and cd into it (the template will use it as project name)
6. Run `dotnet new BlazorHero.CleanArchitecture` to create a new Solution with all the Awesomeness 🕶️ of BlazorHero 🦸

What to do next? Read the [entire guide on my blog](https://codewithmukesh.com/blog/blazor-hero-quick-start-guide/).

## Getting Started with Docker in Windows :rocket:

- Install Docker on Windows via `https://docs.docker.com/docker-for-windows/install/`
- Open up Powershell on Windows and run the following
    - `cd c:\`
    - `dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p securePassword123`
    - `dotnet dev-certs https --trust`
    - Note - Make sure that you use the same password that has been configured in the `docker-compose.yml` file. By default, `securePassword123` is configured.
- 5005 & 5006 are the ports setup to run blazorHero on Docker, so make sure that these ports are free. You could also change the ports in the `docker-compose.yml` and `Server\Dockerfile` files.
- Now navigate back to the root of the BlazorHero Project on your local machine and run the following via terminal - `docker-compose -f 'docker-compose.yml' up --build`
- This will start pulling MSSQL Server Image from Docker Hub if you don't already have this image. It's around 500+ Mbs of download.
- Once that is done, dotnet SDKs and runtimes are downloaded, if not present already. That's almost 200+ more Mbs of download.
- PS If you find any issues while Docker installs the nuget packages, it is most likely that your ssl certificates are not installed properly. Apart from that I also added the `--disable-parallel` in the `Server\Dockerfile`to ensure network issues don't pop-up. You can remove this option to speed up the build process.
- That's almost everything. Once the containers are available, migrations are updated in the MSSQL DB, default data is seeded.
- Browse to https://localhost:5005/ to use your version of BlazorHero !

# Complete Documentation :rocket:

Getting started with Blazor Hero – A Clean Architecture Template built for Blazor WebAssembly using MudBlazor Components. This project will make your Blazor Learning Process much easier than you anticipate. Blazor Hero is meant to be an Enterprise Level Boilerplate, which comes free of cost, completely open sourced. 

The provided documentation / guide will get you started with BlazorHero in no time. It provides a complete walkthrough for the project with to-the-point guides and notes.

<a href="https://codewithmukesh.com/blog/blazor-hero-quick-start-guide/"><strong>Read the Quick Start Guide</strong></a>

# Features

All the completed and the upcoming features are mentioned in the [Features.MD File](https://github.com/blazorhero/CleanArchitecture/blob/master/Features.md)
