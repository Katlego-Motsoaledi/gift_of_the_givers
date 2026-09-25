CREATE TABLE Incidents ( IncidentID INT IDENTITY(1,1) PRIMARY KEY,
  IncidentName NVARCHAR(150) NOT NULL, IncidentType NVARCHAR(100) NOT NULL,
  Location NVARCHAR(255) NOT NULL, StartDate DATETIME2 NOT NULL,
  Status NVARCHAR(50) NOT NULL DEFAULT 'Active' );
CREATE TABLE ReliefCentres ( ReliefCentreID INT IDENTITY(1,1) PRIMARY KEY,
  CentreName NVARCHAR(150) NOT NULL, Location NVARCHAR(255) NOT NULL,
  Capacity INT NOT NULL, IncidentID INT NOT NULL REFERENCES Incidents(IncidentID) );
CREATE TABLE Volunteers ( VolunteerID INT IDENTITY(1,1) PRIMARY KEY,
  FirstName NVARCHAR(100) NOT NULL, LastName NVARCHAR(100) NOT NULL,
  Email NVARCHAR(150) NOT NULL, Phone NVARCHAR(20) NOT NULL,
  SkillSet NVARCHAR(255) NOT NULL, Availability NVARCHAR(100) NOT NULL,
  RegisteredDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
  Status NVARCHAR(50) NOT NULL DEFAULT 'Available' );
CREATE TABLE VolunteerAssignments ( AssignmentID INT IDENTITY(1,1) PRIMARY KEY,
  VolunteerID INT NOT NULL REFERENCES Volunteers(VolunteerID),
  IncidentID  INT NOT NULL REFERENCES Incidents(IncidentID),
  RoleAssigned NVARCHAR(100) NOT NULL,
  AssignmentDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME() );
-- the exact indexes Section B promised
CREATE INDEX IX_Incidents_Status ON Incidents(Status);
CREATE INDEX IX_Incidents_Location ON Incidents(Location);
CREATE INDEX IX_VA_Covering ON VolunteerAssignments(IncidentID, VolunteerID);