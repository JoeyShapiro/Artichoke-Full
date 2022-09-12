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
    action    enum ('add', 'remove', 'modify', 'collect') not null,
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
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'White', 1, DEFAULT, '2022-09-09 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Pumpkin', 1, 1, '2022-09-09 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Wheat', 1, 1, '2022-09-09 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Crossaint', 3, DEFAULT, '2022-09-09 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Fries', 4, DEFAULT, '2022-09-09 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Frozen Breakfast', 4, DEFAULT, '2022-09-09 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Fries', 4, DEFAULT, '2022-09-09 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Gold Fish', 5, 1, '2022-09-11 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Crackers', 5, 1, '2022-09-11 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Crackers', 5, 1, '2022-09-11 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Little Chips', 6, 1, '2022-09-11 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Peanuts', 6, 1, '2022-09-11 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Slim Jim', 6, 1, '2022-09-11 22:11:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Tomato Soup', 5, 1, '2022-09-11 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Ham', 1, DEFAULT, '2022-09-11 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Turkey', 1, 1, '2022-09-11 22:16:05');
INSERT INTO artichoke.items (family_id, item, category_id, collected, modified_on)
    VALUES (1, 'Chicken', 1, 1, '2022-09-11 22:16:05');