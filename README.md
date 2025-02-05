# MovieList

MovieList is an unfinished development project aimed at managing and displaying lists of movies. This project is written primarily in C# and includes a frontend built with TypeScript. Please note that this project is not actively maintained anymore and may contain bugs.

## Table of Contents
- [Getting Started](#getting-started)
- [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgements](#acknowledgements)

## Getting Started
To get started with the MovieList project, you will need to clone the repository and set up your development environment.

### Prerequisites
Ensure you have the following installed:
- Node.js (>= 20.0.0)
- npm
- .NET SDK (>= 6.0)

### Installation
Clone the repository:
```sh
git clone https://github.com/Marijn-de-Mul/MovieList.git
cd MovieList
```

Navigate to the frontend directory and install dependencies:

```sh
cd ML.Frontend
npm install
```

Navigate to the backend directory and restore dependencies:

```sh
cd ../ML.Backend
dotnet restore
```

### Usage
To run the frontend development server:

```sh
cd ML.Frontend
npm run dev
```
To build the frontend project for production:

```sh
npm run build
```

To start the frontend application in production mode:

```sh
npm start
```

To run the backend server:

```sh
cd ../ML.Backend
dotnet run
```

## Project Structure

The main folders and files in the repository are as follows:

ML.Frontend/: Contains the frontend part of the application.
ML.Frontend/package.json: Lists the project dependencies and scripts.
ML.Frontend/README.md: Contains specific instructions for the frontend.
ML.Backend/: Contains the backend part of the application.
ML.Backend/README.md: Contains specific instructions for the backend.

## Contributing

Contributions are welcome! Please read the contributing guidelines to get started.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgements

Thanks to all the contributors who have helped make this project better.

