/*
---------------------------
--Inserción de los sistemas
---------------------------
*/

INSERT INTO "t_sistema"(
            "id_sistema", "nombre")
    VALUES (1, 'PLC1');

INSERT INTO "t_sistema"(
            "id_sistema", "nombre")
    VALUES (2, 'PLC2');

INSERT INTO "t_sistema"(
            "id_sistema", "nombre")
    VALUES (3, 'PLC3');

/*
---------------------------
--Inserción de los botones
---------------------------
*/

--Instante actual

    INSERT INTO "t_registro"(    
            "id_instante", instante)
    VALUES (0, null);

    
/*
---------------------------
--Inserción de los botones
---------------------------
*/

--Sistema 1

INSERT INTO "t_botones"(
            "id_boton", "ref_sistema", "value", "ref_instante")
    VALUES (1, 1 , B'0', 0);
    
INSERT INTO "t_botones"(
            "id_boton", "ref_sistema", "value", "ref_instante")
    VALUES (2, 1, B'0', 0);
    

--Sistema 2

INSERT INTO "t_botones"(
            "id_boton", "ref_sistema", "value", "ref_instante")
    VALUES (1, 2, B'0', 0);
    
INSERT INTO "t_botones"(
            "id_boton", "ref_sistema", "value", "ref_instante")
    VALUES (2, 2, B'0', 0);
    

--Sistema 3

INSERT INTO "t_botones"(
            "id_boton", "ref_sistema", "value", "ref_instante")
    VALUES (1, 3, B'0', 0);
    
INSERT INTO "t_botones"(
            "id_boton", "ref_sistema", "value", "ref_instante")
    VALUES (2, 3, B'0', 0);

/*
---------------------------
--Inserción de los Leds
---------------------------
*/

--Sistema 1


INSERT INTO "t_led"(
            "ref_sistema", "id_led", "value", "ref_instante")
    VALUES (1, 1, B'0', 0);

INSERT INTO "t_led"(
            "ref_sistema", "id_led", "value", "ref_instante")
    VALUES (1, 2, B'0', 0);


--Sistema 2

INSERT INTO "t_led"(
            "ref_sistema", "id_led", "value", "ref_instante")
    VALUES (2, 1, B'0', 0);

INSERT INTO "t_led"(
            "ref_sistema", "id_led", "value", "ref_instante")
    VALUES (2, 2, B'0', 0);

    
--Sistema 3

INSERT INTO "t_led"(
            "ref_sistema", "id_led", "value", "ref_instante")
    VALUES (3, 1, B'0', 0);

INSERT INTO "t_led"(
            "ref_sistema", "id_led", "value", "ref_instante")
    VALUES (3, 2, B'0', 0);

    
/*
---------------------------
--Inserción de los Leds
---------------------------
*/

--Sistema 1

INSERT INTO "t_potenciometro"(
            "id_potenciometro", "ref_sistema", "value", "ref_instante")
    VALUES (1, 1, 0, 0);

INSERT INTO "t_potenciometro"(
            "id_potenciometro", "ref_sistema", "value", "ref_instante")
    VALUES (2, 1, 0, 0);
    
--Sistema 2

INSERT INTO "t_potenciometro"(
            "id_potenciometro", "ref_sistema", "value", "ref_instante")
    VALUES (1, 2, 0, 0);

INSERT INTO "t_potenciometro"(
            "id_potenciometro", "ref_sistema", "value", "ref_instante")
    VALUES (2, 2, 0, 0);

    
--Sistema 3

INSERT INTO "t_potenciometro"(
            "id_potenciometro", "ref_sistema", "value", "ref_instante")
    VALUES (1, 3, 0, 0);

INSERT INTO "t_potenciometro"(
            "id_potenciometro", "ref_sistema", "value", "ref_instante")
    VALUES (2, 3, 0, 0);