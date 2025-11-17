-- Adminer 5.4.1 PostgreSQL 18.0 dump

DROP DATABASE IF EXISTS "jatetxea";
CREATE DATABASE "jatetxea";
\connect "jatetxea";

CREATE TYPE "erabiltzailemotak" AS ENUM ('admin', 'arrunta');

CREATE TYPE "janorduak" AS ENUM ('bazkaria', 'afaria');

DROP TABLE IF EXISTS "Erabiltzaileak";
DROP SEQUENCE IF EXISTS "Erabiltzaileak_id_seq";
CREATE SEQUENCE "Erabiltzaileak_id_seq" INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 START 14 CACHE 1;

CREATE TABLE "public"."Erabiltzaileak" (
    "id" integer DEFAULT nextval('"Erabiltzaileak_id_seq"') NOT NULL,
    "izena" character(50) NOT NULL,
    "pasahitza" character(150) NOT NULL,
    "mota" erabiltzailemotak NOT NULL,
    CONSTRAINT "Erabiltzaileak_pkey" PRIMARY KEY ("id")
)
WITH (oids = false);

CREATE UNIQUE INDEX "Erabiltzaileak_izena" ON public."Erabiltzaileak" USING btree (izena);

INSERT INTO "Erabiltzaileak" ("id", "izena", "pasahitza", "mota") VALUES
(1,	'admin                                             ',	'admin                                                                                                                                                 ',	'admin'),
(2,	'user                                              ',	'user                                                                                                                                                  ',	'arrunta');

DROP TABLE IF EXISTS "Erreserbak";
CREATE TABLE "public"."Erreserbak" (
    "data" date NOT NULL,
    "janordua" janorduak NOT NULL,
    "mahaia" integer NOT NULL,
    "erabiltzailea" integer NOT NULL,
    CONSTRAINT "Erreserbak_data_janordua_mahaia" PRIMARY KEY ("data", "janordua", "mahaia")
)
WITH (oids = false);


DROP TABLE IF EXISTS "Produktuak";
DROP SEQUENCE IF EXISTS "Produktuak_id_seq";
CREATE SEQUENCE "Produktuak_id_seq" INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 START 24 CACHE 1;

CREATE TABLE "public"."Produktuak" (
    "id" integer DEFAULT nextval('"Produktuak_id_seq"') NOT NULL,
    "izena" character(50) NOT NULL,
    "mota" character(50) NOT NULL,
    "prezioa" money DEFAULT '$0.00' NOT NULL,
    "stock" integer DEFAULT '0' NOT NULL,
    CONSTRAINT "Produktuak_pkey" PRIMARY KEY ("id")
)
WITH (oids = false);

INSERT INTO "Produktuak" ("id", "izena", "mota", "prezioa", "stock") VALUES
(23,	'Tinto                                             ',	'Ardoa                                             ',	'$0.00',	0),
(2,	'Kafe Deskafeinatua                                ',	'Kafea                                             ',	'$1.50',	15),
(4,	'Patata Tortila                                    ',	'Tortila                                           ',	'$2.50',	40),
(3,	'Kafe Esnea                                        ',	'Kafea                                             ',	'$1.50',	20),
(7,	'Coca-Cola                                         ',	'Edaria                                            ',	'$2.00',	30),
(8,	'Garagardoa                                        ',	'Edaria                                            ',	'$1.50',	60),
(9,	'Aquarius                                          ',	'Edaria                                            ',	'$2.00',	25),
(1,	'Kafe Amerikanoa                                   ',	'Kafea                                             ',	'$1.00',	10),
(5,	'Txorizo Tortila                                   ',	'Tortila                                           ',	'$2.50',	30),
(6,	'Piper Tortila                                     ',	'Tortila                                           ',	'$2.50',	30);

ALTER TABLE ONLY "public"."Erreserbak" ADD CONSTRAINT "Erreserbak_erabiltzailea_fkey" FOREIGN KEY (erabiltzailea) REFERENCES "Erabiltzaileak"(id) ON UPDATE CASCADE ON DELETE CASCADE NOT DEFERRABLE;

-- 2025-11-17 09:13:16 UTC
