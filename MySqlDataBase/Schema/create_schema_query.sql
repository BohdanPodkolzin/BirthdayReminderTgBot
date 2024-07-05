-- CREATE SCHEMA first_schema;
-- USE first_schema;

-- CREATE TABLE users_place_coords (
--   telegram_id BIGINT NOT NULL,
--   latitude VARCHAR(255) NOT NULL,
--   longitude VARCHAR(255) NOT NULL,
--   PRIMARY KEY (telegram_id)
-- );

-- CREATE TABLE users_schedule (
--   id INT AUTO_INCREMENT PRIMARY KEY,
--   user_telegram_id BIGINT NOT NULL,
--   record VARCHAR(255) NOT NULL,
--   bday_date DATE NOT NULL,
--   FOREIGN KEY (user_telegram_id) REFERENCES users_place_coords (telegram_id)
--   ON DELETE CASCADE
-- );