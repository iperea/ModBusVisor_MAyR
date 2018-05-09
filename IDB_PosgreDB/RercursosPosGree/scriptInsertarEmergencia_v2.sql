-- Function: "insertar_emergencia"(timestamp with time zone, integer, integer, text)

-- DROP FUNCTION "insertar_emergencia"(timestamp with time zone, integer, integer, text);

CREATE OR REPLACE FUNCTION "insertar_emergencia"(dtime timestamp with time zone, idp integer, "idsis" integer, descripcion text)
  RETURNS text AS
$BODY$
DECLARE
VAL integer;
	BEGIN
		val:= nextval('"Registro_idInstante_seq"'::regclass);
		INSERT INTO "t_emergencia"("id_instante", instante, "ref_sistema", descripcion) VALUES (val, dtime, "idsis", descripcion);  
		RETURN 'OK';
	END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "insertar_emergencia"(timestamp with time zone, integer, integer, text) OWNER TO hmiuser;


