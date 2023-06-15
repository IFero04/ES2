-- Inserção na tabela 'utilizador'
INSERT INTO public.utilizador (nome, email, username, senha, telefone, tipo)
VALUES
    ('Afonso', 'af@ipvc.pt', 'af', '123', '111111111', 'Organizador'),
    ('Sonia', 'so@ipvc.pt', 'so', '123', '222222222', 'Participante'),
    ('Jorge', 'jo@ipvc.pt', 'jo', '123', '333333333', 'Participante');

-- Inserção na tabela 'evento'
INSERT INTO evento (nome, data, hora, local, descricao, categoria, capacidade, id_organizador)
VALUES
    ('Concerto de Rock', '2023-07-10', '20:00', 'Teatro Municipal', 'Um concerto emocionante de rock com várias bandas locais.', 'Música', 500, (SELECT id FROM public.utilizador WHERE username = 'af')),
    ('Feira de Artesanato', '2023-06-25', '14:00', 'Praça Central', 'Uma feira com diversos artesãos expondo suas obras.', 'Artesanato', 200, (SELECT id FROM public.utilizador WHERE username = 'so'));

-- Inserção na tabela 'ingresso'
INSERT INTO ingresso (nome, preco, quantidade, id_evento)
VALUES
    ('Ingresso Padrão', 50.00, 100, (SELECT id FROM evento WHERE nome = 'Concerto de Rock')),
    ('Ingresso VIP', 100.00, 50, (SELECT id FROM evento WHERE nome = 'Concerto de Rock')),
    ('Ingresso Único', 20.00, 150, (SELECT id FROM evento WHERE nome = 'Feira de Artesanato'));

-- Inserção na tabela 'atividade'
INSERT INTO atividade (nome, data, hora, descricao, id_evento)
VALUES
    ('Palestra sobre Tecnologia', '2023-07-12', '19:30', 'Uma palestra informativa sobre as últimas tendências tecnológicas.', (SELECT id FROM evento WHERE nome = 'Concerto de Rock')),
    ('Oficina de Pintura', '2023-06-26', '15:00', 'Aprenda técnicas de pintura com um artista renomado.', (SELECT id FROM evento WHERE nome = 'Feira de Artesanato'));

-- Inserção na tabela 'inscricao_evento'
INSERT INTO inscricao_evento (id_evento, id_participante, tipo_ingresso)
VALUES
    ((SELECT id FROM evento WHERE nome = 'Concerto de Rock'), (SELECT id FROM public.utilizador WHERE username = 'jo'), (SELECT id FROM ingresso WHERE nome = 'Ingresso Único')),
    ((SELECT id FROM evento WHERE nome = 'Feira de Artesanato'), (SELECT id FROM public.utilizador WHERE username = 'jo'), (SELECT id FROM ingresso WHERE nome = 'Ingresso Padrão'));

-- Inserção na tabela 'inscricao_atividade'
INSERT INTO inscricao_atividade (id_atividade, id_participante)
VALUES
    ((SELECT id FROM atividade WHERE nome = 'Palestra sobre Tecnologia'), (SELECT id FROM public.utilizador WHERE username = 'jo'));

-- Inserção na tabela 'feedback'
INSERT INTO feedback (comentario, id_inscricao)
VALUES
    ('O evento foi muito bem organizado. Parabéns!', (SELECT id FROM inscricao_evento WHERE id_evento = (SELECT id FROM evento WHERE nome = 'Concerto de Rock'))),
    ('Achei o preço do ingresso um pouco alto.', (SELECT id FROM inscricao_evento WHERE id_evento = (SELECT id FROM evento WHERE nome = 'Feira de Artesanato')));

-- Inserção na tabela 'mensagem'
INSERT INTO mensagem (mensagem, id_evento)
VALUES
    ('Olá! Gostaríamos de convidá-lo para participar da nossa atividade.', (SELECT id FROM evento WHERE nome = 'Concerto de Rock')),
    ('Obrigado por participar do nosso evento! Esperamos que você tenha se divertido.', (SELECT id FROM evento WHERE nome = 'Feira de Artesanato'));
