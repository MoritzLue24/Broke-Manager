default:
    @just --list


start:
    docker compose up -f docker-compose.dev.yml -d

stop:
    docker compose -f docker-compose.dev.yml down

#falls dockerfile verändert wurde
rebuild:
    docker compose up f docker-compose.dev.yml -d --build

logs:
    docker-compose -f docker-compose.dev.yml logs -f