-- Exported from QuickDBD: https://www.quickdatatabasediagrams.com/
-- Link to schema: https://app.quickdatabasediagrams.com/#/schema/ZkgjfXI-BESN4vB9pg5rQA
-- NOTE! If you have used non-SQL datatypes in your design, you will have to change these here.

CREATE DATABASE card_catalog
GO
USE [card_catalog]
GO

CREATE TABLE "books" (
    "id" int  NOT NULL ,
    "title" VARCHAR(255)  NOT NULL ,
    CONSTRAINT "pk_books" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "authors" (
    "id" int  NOT NULL ,
    "name" VARCHAR(255)  NOT NULL ,
    CONSTRAINT "pk_authors" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "patrons" (
    "id" int  NOT NULL ,
    "name" VARCHAR(255)  NOT NULL ,
    CONSTRAINT "pk_patrons" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "authors_books" (
    "id" int  NOT NULL ,
    "author_id" int  NOT NULL ,
    "book_id" int  NOT NULL ,
    CONSTRAINT "pk_authors_books" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "copies" (
    "id" int  NOT NULL ,
    "book_id" int  NOT NULL ,
    CONSTRAINT "pk_copies" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "checkouts" (
    "id" int  NOT NULL ,
    "patron_id" int  NOT NULL ,
    "copy_id" int  NOT NULL ,
    "due_date" datetime  NOT NULL ,
    CONSTRAINT "pk_checkouts" PRIMARY KEY (
        "id"
    )
)

GO

ALTER TABLE "authors_books" ADD CONSTRAINT "fk_authors_books_author_id" FOREIGN KEY("author_id")
REFERENCES "authors" ("id")
GO

ALTER TABLE "authors_books" ADD CONSTRAINT "fk_authors_books_book_id" FOREIGN KEY("book_id")
REFERENCES "books" ("id")
GO

ALTER TABLE "copies" ADD CONSTRAINT "fk_copies_book_id" FOREIGN KEY("book_id")
REFERENCES "books" ("id")
GO

ALTER TABLE "checkouts" ADD CONSTRAINT "fk_checkouts_patron_id" FOREIGN KEY("patron_id")
REFERENCES "patrons" ("id")
GO

ALTER TABLE "checkouts" ADD CONSTRAINT "fk_checkouts_copy_id" FOREIGN KEY("copy_id")
REFERENCES "copies" ("id")
GO

