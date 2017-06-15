-- Exported from QuickDBD: https://www.quickdatatabasediagrams.com/
-- Link to schema: https://app.quickdatabasediagrams.com/#/schema/ZkgjfXI-BESN4vB9pg5rQA
-- NOTE! If you have used non-SQL datatypes in your design, you will have to change these here.

CREATE DATABASE card_catalog
GO
USE [card_catalog]
GO

CREATE TABLE "books" (
    "id" int IDENTITY(1,1) NOT NULL ,
    "title" VARCHAR(255)  NOT NULL ,
    CONSTRAINT "pk_books" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "authors" (
    "id" int IDENTITY(1,1) NOT NULL ,
    "name" VARCHAR(255)  NOT NULL ,
    CONSTRAINT "pk_authors" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "patrons" (
    "id" int IDENTITY(1,1) NOT NULL ,
    "name" VARCHAR(255)  NOT NULL ,
    CONSTRAINT "pk_patrons" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "authors_books" (
    "id" int IDENTITY(1,1) NOT NULL ,
    "author_id" int  NOT NULL ,
    "book_id" int  NOT NULL ,
    CONSTRAINT "pk_authors_books" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "copies" (
    "id" int IDENTITY(1,1) NOT NULL ,
    "book_id" int  NOT NULL ,
    "due_date" VARCHAR(50) ,
    CONSTRAINT "pk_copies" PRIMARY KEY (
        "id"
    )
)

GO

CREATE TABLE "checkouts" (
    "id" int IDENTITY(1,1) NOT NULL ,
    "patron_id" int  NOT NULL ,
    "copy_id" int  NOT NULL ,
	"returned" bit DEFAULT 0 ,
    CONSTRAINT "pk_checkouts" PRIMARY KEY (
        "id"
    )
)

GO

