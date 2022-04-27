create schema cad;

create sequence cad.diretor_seq;
create table cad.diretor (
    id int not null default nextval('cad.diretor_seq'),
    nome varchar not null,
    constraint diretor_pk primary key (id)
);

create sequence cad.filme_seq;
create table cad.filme (
    id int not null default nextval('cad.filme_seq'),
    id_diretor int not null,
    nome varchar not null,
    constraint filme_pk primary key (id),
    constraint filme_diretor_pk foreign key (id_diretor) references cad.diretor(id)
);