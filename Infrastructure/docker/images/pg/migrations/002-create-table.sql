CREATE TABLE utilizador (
     id             uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
     nome           VARCHAR(100) NOT NULL,
     email          VARCHAR(100) NOT NULL,
     username       VARCHAR(50) NOT NULL,
     senha          VARCHAR(50) NOT NULL,
     telefone       VARCHAR(20),
     tipo           VARCHAR(100) NOT NULL
);

CREATE TABLE evento (
    id              uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    nome            VARCHAR(100) NOT NULL,
    data            Date NOT NULL,
    hora            VARCHAR(100) NOT NULL,
    local           VARCHAR(100) NOT NULL,
    descricao       VARCHAR(500),
    categoria       VARCHAR(100) NOT NULL,
    capacidade      INTEGER NOT NULL,
    id_organizador  uuid REFERENCES utilizador(id) NOT NULL
);

CREATE TABLE mensagem (
    id              uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    mensagem        VARCHAR(500) NOT NULL,
    id_evento       uuid REFERENCES evento(id) NOT NULL
);

CREATE TABLE ingresso(
    id              uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    nome            VARCHAR(100) NOT NULL,
    preco           FLOAT NOT NULL,
    quantidade      INTEGER NOT NULL,
    id_evento       uuid REFERENCES evento(id) NOT NULL
);

CREATE TABLE atividade(
    id              uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    nome            VARCHAR(100) NOT NULL,
    data            Date NOT NULL,
    hora            VARCHAR(100) NOT NULL,
    descricao       VARCHAR(500),
    id_evento       uuid REFERENCES evento(id) NOT NULL
);

CREATE TABLE inscricao_evento(
    id              uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    id_evento       uuid REFERENCES evento(id) NOT NULL,
    id_participante uuid REFERENCES utilizador(id) NOT NULL,
    tipo_ingresso   uuid REFERENCES ingresso(id) NOT NULL              
);

CREATE TABLE feedback(
    id              uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    comentario      VARCHAR(500) NOT NULL,
    id_inscricao    uuid REFERENCES inscricao_evento(id) NOT NULL
);

CREATE TABLE  inscricao_atividade(
    id              uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    id_atividade    uuid REFERENCES atividade(id) NOT NULL,
    id_participante uuid REFERENCES utilizador(id) NOT NULL                  
);