-- Create tables

CREATE TABLE Teams (
    team_id INTEGER NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    name_abbr VARCHAR(4) NOT NULL,
    nationality VARCHAR(255) NOT NULL,
    budget INTEGER NOT NULL
);

CREATE TABLE Riders (
    rider_id INTEGER NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    birthday DATE NOT NULL,
    salary INT NOT NULL,
    rider_type VARCHAR(50) NOT NULL,
    nationality VARCHAR(255) NOT NULL,
    team_id INTEGER,
    FOREIGN KEY (team_id) REFERENCES Teams(team_id)
);




