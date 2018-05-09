-- View: vista_historial_botones

-- DROP VIEW vista_historial_botones;

CREATE OR REPLACE VIEW vista_historial_botones AS 
 SELECT t_registro.instante, t_sistema.nombre, t_led.id_led, t_led.value
   FROM t_led, t_registro, t_sistema
  WHERE t_led.ref_sistema = t_sistema.id_sistema AND t_registro.id_instante = t_led.ref_instante AND t_registro.id_instante <> 0;

ALTER TABLE vista_historial_botones OWNER TO postgres;
GRANT ALL ON TABLE vista_historial_botones TO postgres;
GRANT SELECT ON TABLE vista_historial_botones TO public;

