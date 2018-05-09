-- Sequence: "Registro_idInstante_seq"

-- DROP SEQUENCE "Registro_idInstante_seq";

CREATE SEQUENCE "Registro_idInstante_seq"
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 147
  CACHE 1;
ALTER TABLE "Registro_idInstante_seq" OWNER TO hmiuser;


-- Table: t_registro

-- DROP TABLE t_registro;

CREATE TABLE t_registro
(
  id_instante bigint NOT NULL DEFAULT nextval('"Registro_idInstante_seq"'::regclass),
  instante timestamp with time zone,
  CONSTRAINT "Registro_pkey" PRIMARY KEY (id_instante)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE t_registro OWNER TO hmiuser;


-- Table: t_sistema

-- DROP TABLE t_sistema;

CREATE TABLE t_sistema
(
  id_sistema integer NOT NULL,
  nombre character(20),
  CONSTRAINT "idTabla" PRIMARY KEY (id_sistema)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE t_sistema OWNER TO hmiuser;



-- Table: t_botones

-- DROP TABLE t_botones;

CREATE TABLE t_botones
(
  id_boton integer NOT NULL,
  ref_sistema integer NOT NULL,
  "value" bit(1),
  ref_instante bigint NOT NULL,
  CONSTRAINT "Botones_pkey" PRIMARY KEY (id_boton, ref_sistema, ref_instante),
  CONSTRAINT "Botones_refInstante_fkey" FOREIGN KEY (ref_instante)
      REFERENCES t_registro (id_instante) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT "Botones_refSistema_fkey" FOREIGN KEY (ref_sistema)
      REFERENCES t_sistema (id_sistema) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE t_botones OWNER TO hmiuser;


-- Table: t_emergencia

-- DROP TABLE t_emergencia;

CREATE TABLE t_emergencia
(
-- Heredado from table t_registro:  id_instante bigint NOT NULL DEFAULT nextval('"Registro_idInstante_seq"'::regclass),
-- Heredado from table t_registro:  instante timestamp with time zone,
  ref_sistema integer NOT NULL,
  descripcion text NOT NULL,
  CONSTRAINT "Emergencia_pkey" PRIMARY KEY (id_instante),
  CONSTRAINT "Emergencia_refSistema_fkey" FOREIGN KEY (ref_sistema)
      REFERENCES t_sistema (id_sistema) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
INHERITS (t_registro)
WITH (
  OIDS=FALSE
);
ALTER TABLE t_emergencia OWNER TO postgres;


-- Table: t_led

-- DROP TABLE t_led;

CREATE TABLE t_led
(
  ref_sistema integer NOT NULL,
  id_led integer NOT NULL,
  "value" bit(1),
  ref_instante bigint NOT NULL,
  CONSTRAINT "Led_pkey" PRIMARY KEY (ref_sistema, id_led, ref_instante),
  CONSTRAINT "Led_refInstante_fkey" FOREIGN KEY (ref_instante)
      REFERENCES t_registro (id_instante) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT "Led_refSistema_fkey" FOREIGN KEY (ref_sistema)
      REFERENCES t_sistema (id_sistema) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE t_led OWNER TO hmiuser;



-- Table: t_potenciometro

-- DROP TABLE t_potenciometro;

CREATE TABLE t_potenciometro
(
  id_potenciometro integer NOT NULL,
  ref_sistema integer NOT NULL,
  "value" real,
  ref_instante bigint NOT NULL,
  CONSTRAINT "Potenciometro_pkey" PRIMARY KEY (id_potenciometro, ref_sistema, ref_instante),
  CONSTRAINT "Potenciometro_refInstante_fkey" FOREIGN KEY (ref_instante)
      REFERENCES t_registro (id_instante) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT "Potenciometro_refSistema_fkey" FOREIGN KEY (ref_sistema)
      REFERENCES t_sistema (id_sistema) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE t_potenciometro OWNER TO hmiuser;




