-- Function: actualizar_botones(timestamp with time zone, integer, integer, bit)

-- DROP FUNCTION actualizar_botones(timestamp with time zone, integer, integer, bit);

CREATE OR REPLACE FUNCTION actualizar_botones(idb integer, idsis integer, valor bit)
  RETURNS text AS
$BODY$
DECLARE
VAL integer;
	BEGIN
		UPDATE t_botones set value=valor where ref_sistema=idsis AND ref_instante=0 AND id_boton=idb;
		    RETURN 'OK';
	END;


$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
  
ALTER FUNCTION actualizar_botones( integer, integer, bit) OWNER TO hmiuser;


-- Function: actualizar_botones(timestamp with time zone, integer, integer, bit)

--DROP FUNCTION registrar_cambio_botones();

CREATE OR REPLACE FUNCTION registrar_cambio_botones()
  RETURNS TRIGGER AS
$BODY$
DECLARE
VAL integer;
	BEGIN		
		val:= nextval('"Registro_idInstante_seq"'::regclass);
		INSERT INTO "t_registro"("id_instante", instante) VALUES (val, current_timestamp);
		INSERT INTO "t_botones"("id_boton", "ref_sistema", value, "ref_instante")
		    VALUES (OLD.id_boton, OLD.ref_sistema, NEW.value , val);
		    
		    RETURN null;
	END;


$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

-- Triger que cuando hay un cambio en los valores lo registra en el historico
--Se borra por si existía previamente
DROP TRIGGER IF EXISTS actualiza_fecha_boton on t_botones;


CREATE TRIGGER actualiza_fecha_boton AFTER UPDATE
ON t_botones FOR EACH ROW
 WHEN (OLD.ref_instante=0 AND NEW.ref_instante=0 AND OLD.value<>NEW.value)
EXECUTE PROCEDURE registrar_cambio_botones( );

