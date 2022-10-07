-- api user account
create user rest_api;
grant execute on artichoke.* to rest_api;

-- create items table
create table items
(
    id        int auto_increment,
    family_id int                not null,
    item      varchar(32)        not null,
    category  varchar(16)        not null,
    collected bool default false not null,
    constraint items_pk
        primary key (id)
);

create unique index items_id_uindex
    on items (id);

-- create families table
create table families
(
    id              int auto_increment,
    family_name     varchar(32)  not null,
    passphrase_hash varchar(256) not null,
    sub_expires_on  int          not null,
    constraint families_pk
        primary key (id)
);

create unique index families_id_uindex
    on families (id);

-- create users table
create table users
(
    id        int auto_increment,
    username  varchar(32) not null,
    family_id int         not null,
    constraint users_pk
        primary key (id)
);

create unique index users_id_uindex
    on users (id);

-- create logs tables
create table logs
(
    id        int auto_increment,
    family_id int                                         not null,
    user_id   int                                         not null,
    action    enum ('add', 'remove', 'modify', 'collected') not null,
    item_id   int                                         not null,
    constraint logs_pk
        primary key (id)
);

create unique index logs_id_uindex
    on logs (id);

create unique index users_id_uindex
    on logs (id);

-- create categories table
create table categories
(
    id        int auto_increment,
    category  varchar(32) not null,
    family_id int         not null,
    constraint categories_pk
        primary key (id)
);

create unique index categories_id_uindex
    on categories (id);

-- add datetime to items and logs
alter table items
    add modified_on datetime not null;
alter table logs
    add modified_on datetime not null;

-- convert to id
alter table items
    change category category_id int not null;

-- maybe add notes to items

-- stored procedures
-- add item
DELIMITER //
CREATE PROCEDURE item_add (IN given_family_id int, IN given_user_id int, IN given_item varchar(32), IN given_category_id int)
    BEGIN
        SET @now_time := NOW();

        INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
            VALUES (given_family_id, given_item, given_category_id, DEFAULT, @now_time);

        -- this should be more than enough
        -- but really date should be more than enough
        SET @current_item_id := (SELECT id FROM items WHERE modified_on=current_time
                                                        AND family_id=given_family_id AND item=given_item
                                                        AND category_id=given_category_id);
        INSERT INTO artichoke.logs (family_id, user_id, action, item_id, modified_on)
            VALUES (given_family_id, given_user_id, 'add', @current_item_id, @now_time);
    END //

-- collect item
DELIMITER //
CREATE PROCEDURE item_collect (IN given_family_id int, IN given_user_id int, IN given_item_id int)
    BEGIN
        SET @now_time := NOW();

        UPDATE artichoke.items t
        SET t.collected = 1, t.modified_on = @now_time
        WHERE t.id = given_item_id;

        INSERT INTO artichoke.logs (family_id, user_id, action, item_id, modified_on)
            VALUES (given_family_id, given_user_id, 'collect', given_item_id, @now_time);
    END //

-- remove item
DELIMITER //
CREATE PROCEDURE item_remove (IN given_family_id int, IN given_user_id int, IN given_item_id int)
    BEGIN
        SET @now_time := NOW();

        DELETE
        FROM artichoke.items
        WHERE id = given_item_id;

        INSERT INTO artichoke.logs (family_id, user_id, action, item_id, modified_on)
            VALUES (given_family_id, given_user_id, 'remove', given_item_id, @now_time);
    END //

-- get categories of a family
DELIMITER //
CREATE PROCEDURE get_categories (IN given_family_id int)
    BEGIN
        SELECT id, category FROM categories 
        WHERE family_id=given_family_id;
    END //

-- get logs of a family
DELIMITER //
CREATE PROCEDURE get_family_logs (IN given_family_id int)
    BEGIN
        SELECT logs.id, u.username, logs.action, i.item, logs.modified_on FROM logs
            INNER JOIN users u on logs.user_id = u.id
            INNER JOIN items i on logs.item_id = i.id
            WHERE logs.family_id=given_family_id
            ORDER BY modified_on DESC;
    END //

-- get and verify an account
DELIMITER //


-- dummy data
-- families
INSERT INTO artichoke.families (family_name, passphrase_hash, sub_expires_on)
    VALUES ('shapiro', 'sha256', 2147483647);
-- users
INSERT INTO artichoke.users (username, family_id)
    VALUES ('joey', 1);
-- categories
INSERT INTO artichoke.categories (category, family_id)
    VALUES ('Bread', 1);
INSERT INTO artichoke.categories (category, family_id)
    VALUES ('Deli', 1);
INSERT INTO artichoke.categories (category, family_id)
    VALUES ('Pastry', 1);
INSERT INTO artichoke.categories (category, family_id)
    VALUES ('Frozen', 1);
INSERT INTO artichoke.categories (category, family_id)
    VALUES ('Soup', 1);
INSERT INTO artichoke.categories (category, family_id)
    VALUES ('Chips', 1);
-- items
CALL item_add(1, 1, 'White', 1);
CALL item_add(1, 1, 'Pumpkin', 1);
CALL item_add(1, 1, 'Wheat', 1);
CALL item_add(1, 1, 'Crossaint', 1);
CALL item_add(1, 1, 'Fries', 4);
CALL item_add(1, 1, 'Frozen Breakfast', 4);
CALL item_add(1, 1, 'Curly Fries', 4);
CALL item_add(1, 1, 'Gold Fish', 5);
CALL item_add(1, 1, 'Crackers', 5);
CALL item_add(1, 1, 'Little Chips', 6);
CALL item_add(1, 1, 'Peanuts', 6);
CALL item_add(1, 1, 'Slim Jims', 6);
CALL item_add(1, 1, 'Tomato Soup', 5);
CALL item_add(1, 1, 'Ham', 1);
CALL item_add(1, 1, 'Turkey', 1);
CALL item_add(1, 1, 'Chicken', 1);

CALL item_collect(1, 1, 1);
CALL item_collect(1, 1, 2);
CALL item_collect(1, 1, 3);
CALL item_collect(1, 1, 4);
CALL item_collect(1, 1, 5);
CALL item_collect(1, 1, 6);
CALL item_collect(1, 1, 7);
CALL item_collect(1, 1, 8);
CALL item_collect(1, 1, 9);
CALL item_collect(1, 1, 10);
CALL item_collect(1, 1, 11);

-- ? could use this, but found better way
-- -- items
-- DELIMITER #
-- CREATE TRIGGER items_add AFTER INSERT ON items
--     FOR EACH ROW
--     BEGIN
--         INSERT INTO artichoke.logs (family_id, user_id, action, item_id, modified_on)
--             VALUES (NEW.family_id, NEW.last_modified_by, 'add', NEW.id, NEW.modified_on);
--     END #

-- DELIMITER #
-- CREATE TRIGGER items_update AFTER UPDATE ON items
--     FOR EACH ROW
--     BEGIN
--         INSERT INTO artichoke.logs (family_id, user_id, action, item_id, modified_on)
--             VALUES (NEW.family_id, NEW.last_modified_by, 'modify', NEW.id, NEW.modified_on);
--     end #

-- DELIMITER #
-- CREATE TRIGGER items_remove AFTER DELETE ON items
--     FOR EACH ROW
--     BEGIN
--         INSERT INTO artichoke.logs (family_id, user_id, action, item_id, modified_on)
--             VALUES (OLD.family_id, OLD.last_modified_by, 'remove', OLD.id, OLD.modified_on);
--     end #