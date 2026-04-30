default:
    @just --list


start:
    docker compose  -f docker-compose.dev.yml up -d

stop:
    docker compose -f docker-compose.dev.yml down

#falls dockerfile verändert wurde
rebuild:
    docker compose  -f docker-compose.dev.yml up -d --build

logs:
    docker-compose -f docker-compose.dev.yml logs -f