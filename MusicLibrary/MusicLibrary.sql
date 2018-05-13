DROP TABLE IF EXISTS artist;
CREATE TABLE artist 
(
  id int NOT NULL IDENTITY(1,1),
  name varchar(150) NOT NULL,
  create_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  modify_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
  PRIMARY KEY (id)
)

DROP TABLE IF EXISTS genre;
CREATE TABLE genre 
(
  id int NOT NULL IDENTITY(1,1),
  name varchar(150) NOT NULL,
  create_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  modify_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
  PRIMARY KEY (id)
)

DROP TABLE IF EXISTS album;
CREATE TABLE album 
(
  id int NOT NULL IDENTITY(1,1),
  title varchar(150) NOT NULL,
  artist_id int NULL,
  year varchar(4) NULL,
  genre_id int NULL,
  create_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  modify_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  FOREIGN KEY (artist_id) REFERENCES artist(id),
  FOREIGN KEY (genre_id) REFERENCES genre(id)
)

DROP TABLE IF EXISTS path;
CREATE TABLE path 
(
  id int NOT NULL IDENTITY(1,1),
  location varchar(256) NOT NULL,
  create_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  modify_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id)
)

DROP TABLE IF EXISTS track;
CREATE TABLE track 
(
  id int NOT NULL IDENTITY(1,1),
  file_name varchar(256) NOT NULL,
  path_id int NOT NULL,
  title varchar(150) NOT NULL,
  album_id int NOT NULL,
  genre_id int NOT NULL,
  artist_id int NOT NULL,
  number varchar(10) NOT NULL DEFAULT '',
  year varchar(4) NULL,
  length int NULL,
  play_count int NOT NULL DEFAULT 0,
  create_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  modify_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  FOREIGN KEY (path_id) REFERENCES path(id),
  FOREIGN KEY (album_id) REFERENCES album(id),
  FOREIGN KEY (artist_id) REFERENCES artist(id)
)

DROP TABLE IF EXISTS playlist;
CREATE TABLE playlist
(
  id int NOT NULL IDENTITY(1,1),
  name varchar(150) NOT NULL,
  create_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  modify_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id)
) 

DROP TABLE IF EXISTS playlist_track;
CREATE TABLE playlist_track
(
  id int NOT NULL IDENTITY(1,1),
  playlist_id int NOT NULL,
  track_id int NOT NULL,
  create_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  modify_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  FOREIGN KEY (playlist_id) REFERENCES playlist(id),
  FOREIGN KEY (track_id) REFERENCES track(id) 
)

DROP TABLE IF EXISTS app_log;
CREATE TABLE app_log
(
  id int NOT NULL IDENTITY(1,1),
  description TEXT NOT NULL,
  is_error bit NOT NULL DEFAULT 0,
  create_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  modify_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id)
)

DROP TABLE IF EXISTS api_log;
CREATE TABLE api_log
(
  id int NOT NULL IDENTITY(1,1)
  PRIMARY KEY (id)
)