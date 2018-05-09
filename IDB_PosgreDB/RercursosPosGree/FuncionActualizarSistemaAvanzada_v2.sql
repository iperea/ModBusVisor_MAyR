

-- Function: "actualizar_sistema"(timestamp with time zone, integer, integer[], bit[], integer[], bit[], integer[], real[])

-- DROP FUNCTION "actualizar_sistema"(timestamp with time zone, integer, integer[], bit[], integer[], bit[], integer[], real[]);

CREATE OR REPLACE FUNCTION "actualizar_sistema"("idsis" integer, i_id_boton integer[], valor_boton bit[], i_id_led integer[], valor_led bit[], i_id_poten integer[], valor_pot real[])
  RETURNS integer AS
$BODY$
DECLARE
val text;
cant integer;
	BEGIN


		cant:=(select count(*) from "t_botones" where "idsis"="ref_sistema" and "ref_instante"=0);
		FOR i IN 1..cant LOOP
			val:=(SELECT "actualizar_botones"(i_id_boton[i], idsis, valor_boton[i]));			    
		END LOOP;
		    
		cant:=(select count(*) from "t_led" where "idsis"="ref_sistema" and "ref_instante"=0);		
		
		FOR i IN 1..cant LOOP
		
			val:=(SELECT "actualizar_led"(i_id_led[i], idsis, valor_led[i]));
			    		    
		END LOOP;
		
		cant:=(select count(*) from "t_potenciometro" where "idsis"="ref_sistema" and "ref_instante"=0);
		
		FOR i IN 1..cant LOOP
		
			val:=(SELECT "actualizar_potenciometro"(i_id_poten[i], idsis, valor_pot[i]));    
			    		    
		END LOOP;
		
		RETURN 0;
	END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION "actualizar_sistema"( integer, integer[], bit[], integer[], bit[], integer[], real[]) OWNER TO hmiuser;