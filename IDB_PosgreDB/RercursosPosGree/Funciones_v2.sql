-- Function: "actualizar_botones"(timestamp with time zone, integer, integer, bit)

--DROP FUNCTION "actualizar_botones"(timestamp with time zone, integer, integer, bit);

CREATE OR REPLACE FUNCTION "actualizar_botones"(dtime timestamp with time zone, idb integer, "idsis" integer, valor bit)
  RETURNS text AS
$BODY$
DECLARE
VAL integer;
	BEGIN
		val:= nextval('"Registro_idInstante_seq"'::regclass);
		INSERT INTO "t_registro"("id_instante", instante) VALUES (val, dtime);
		INSERT INTO "t_botones"("id_boton", "ref_sistema", value, "ref_instante")
		    VALUES (idB, "idsis", valor, val);
		    RETURN 'OK';
	END;


$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "actualizar_botones"(timestamp with time zone, integer, integer, bit) OWNER TO hmiuser;

-- Function: "actualizar_led"(timestamp with time zone, integer, integer, bit)

--DROP FUNCTION "actualizar_led"(timestamp with time zone, integer, integer, bit);

CREATE OR REPLACE FUNCTION "actualizar_led"(dtime timestamp with time zone, idl integer, "idsis" integer, valor bit)
  RETURNS text AS
$BODY$
DECLARE
VAL integer;
	BEGIN
		val:= nextval('"Registro_idInstante_seq"'::regclass);
		INSERT INTO "t_registro"("id_instante", instante) VALUES (val, dtime);
		INSERT INTO "t_led"("id_led", "ref_sistema", value, "ref_instante")
		    VALUES (idl, "idsis", valor, val);
		    RETURN 'OK';
	END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "actualizar_led"(timestamp with time zone, integer, integer, bit) OWNER TO hmiuser;


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


-- Function: "actualizar_potenciometro"(timestamp with time zone, integer, integer, real)

-- DROP FUNCTION "actualizar_potenciometro"(timestamp with time zone, integer, integer, real);

CREATE OR REPLACE FUNCTION "actualizar_potenciometro"(dtime timestamp with time zone, idp integer, "idsis" integer, valor real)
  RETURNS text AS
$BODY$
DECLARE
VAL integer;
	BEGIN
		val:= nextval('"Registro_idInstante_seq"'::regclass);
		INSERT INTO "t_registro"("id_instante", instante) VALUES (val, dtime);
		INSERT INTO "t_potenciometro"("id_potenciometro", "ref_sistema", "value", ref_instante)
		    VALUES (idp, "idsis", valor, val);
		    RETURN 'OK';
	END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "actualizar_potenciometro"(timestamp with time zone, integer, integer, real) OWNER TO hmiuser;


-- Function: "actualizar_sistema"(timestamp with time zone, integer, integer[], bit[], integer[], bit[], integer[], real[])

-- DROP FUNCTION "actualizar_sistema"(timestamp with time zone, integer, integer[], bit[], integer[], bit[], integer[], real[]);

CREATE OR REPLACE FUNCTION "actualizar_sistema"(dtime timestamp with time zone, "idsis" integer, i_id_boton integer[], valor_boton bit[], i_id_led integer[], valor_led bit[], i_id_poten integer[], valor_pot real[])
  RETURNS integer AS
$BODY$
DECLARE
val integer;
cant integer;
	BEGIN
		val:= nextval('"Registro_idInstante_seq"'::regclass);
		INSERT INTO "t_registro"("id_instante", instante) VALUES (val, dtime);


		cant:=(select count(*) from "t_botones" where "idsis"="ref_sistema" and "ref_instante"=0);
		FOR i IN 1..cant LOOP
					
			INSERT INTO "t_botones"("id_boton", "ref_sistema", value, "ref_instante")
			    VALUES (i_id_boton[i], "idsis", valor_boton[i], val);
			    
		END LOOP;
		    
		cant:=(select count(*) from "t_led" where "idsis"="ref_sistema" and "ref_instante"=0);		
		
		FOR i IN 1..cant LOOP
		
			INSERT INTO "t_led"("id_led", "ref_sistema", value, "ref_instante")
			    VALUES (i_id_led[i], "idsis", valor_led[i], val);
			    		    
		END LOOP;
		
		cant:=(select count(*) from "t_potenciometro" where "idsis"="ref_sistema" and "ref_instante"=0);
		
		FOR i IN 1..cant LOOP
		
			INSERT INTO "t_potenciometro"("id_potenciometro", "ref_sistema", "value", "ref_instante")
			    VALUES (i_id_poten[i], "idsis", valor_pot[i], val);		    
			    		    
		END LOOP;
		
		RETURN 0;
	END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "actualizar_sistema"(timestamp with time zone, integer, integer[], bit[], integer[], bit[], integer[], real[]) OWNER TO hmiuser;