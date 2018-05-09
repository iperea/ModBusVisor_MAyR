-- Function: actualizar_botones(timestamp with time zone, integer, integer, bit)

---DROP FUNCTION actualizar_potenciometro(integer, integer, bit);

CREATE OR REPLACE FUNCTION "actualizar_potenciometro"(idp integer, "idsis" integer, valor real) RETURNS text AS
$$
DECLARE
VAL integer;
	BEGIN
		UPDATE t_potenciometro set value=valor where ref_sistema=idsis AND ref_instante=0 AND id_potenciometro=idp;
		    RETURN 'OK';
	END;
$$
LANGUAGE plpgsql;
ALTER FUNCTION "actualizar_potenciometro"( integer, integer, real) OWNER TO hmiuser;


-- Function: actualizar_botones(timestamp with time zone, integer, integer, bit)

 --DROP FUNCTION registrar_cambio_potenciometros();

CREATE OR REPLACE FUNCTION registrar_cambio_potenciometros()
  RETURNS TRIGGER AS
$BODY$
DECLARE
VAL integer;
	BEGIN		
		val:= nextval('"Registro_idInstante_seq"'::regclass);
		INSERT INTO "t_registro"("id_instante", instante) VALUES (val, current_timestamp);
		INSERT INTO "t_potenciometro"("id_potenciometro", "ref_sistema", value, "ref_instante")
		    VALUES (OLD.id_potenciometro, OLD.ref_sistema, NEW.value , val);
		    RETURN null;
	END;


$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

-- Triger que cuando hay un cambio en los valores lo registra en el historico
--Se borra por si existía previamente
DROP TRIGGER IF EXISTS actualiza_fecha_potenciometro on t_potenciometro;


CREATE TRIGGER actualiza_fecha_potenciometro AFTER UPDATE
ON t_potenciometro FOR EACH ROW
 WHEN (OLD.ref_instante=0 AND NEW.ref_instante=0 AND OLD.value<>NEW.value)
EXECUTE PROCEDURE registrar_cambio_potenciometros();

