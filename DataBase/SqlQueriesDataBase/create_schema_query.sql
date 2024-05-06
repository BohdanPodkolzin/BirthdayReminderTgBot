CREATE SCHEMA telegram_storage;
USE telegram_storage;

CREATE TABLE users_schedule(
	id INT PRIMARY KEY AUTO_INCREMENT,
    user_telegram_id BIGINT NOT NULL,
    human_in_schedule VARCHAR(255) NOT NULL,
    bday_date DATE NOT NULL
);