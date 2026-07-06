# Fixme / Überlegungen

**Was ich anders machen würde:**
* **Mediator pattern overkill?** Brauche aktuell 3+ Dateien für ein einfaches Feature. Das macht es schwieriger, gerade als einzelne Person, neue features zu implementieren. Einfache services würde vielleicht reichen.

* Die App, Ziele, Funktionen & dafür nötigen Datemodelle **besser planen**, & strukturiertere Docs, an denen ich mich während dem Entwickeln orientiere, nicht andersrum. Ich musste im Laufe der Entwicklung viele Sachen ändern, weil ich nicht von Anfang an die richtigen Entscheidungen getroffen habe. Dadurch bin ich teilweise sehr langsam vorangekommen.

* SessionCleanupJob: Sessions würden bei mehreren Api-Instanzen nicht sauber gelöscht werden. Besser wäre ein **seperater CleanupJob**, der auch horizontale Skalierung unterstützt, ist aber auch etwas früh gedacht, diesen punkt wird man bei der aktuellen Größe des Projekts nicht erreichen.

* **Testing besser organisieren:** nicht für jede schicht ein Unit-Test- & eventuell Integration-Test-Projekt, sondern gezielt überlegen wo & welches testing sinnvoll ist. Für jedes Feature viele verschiedene Tests auf jeder ebene zu schreiben ist zu aufwendig.

**Probleme beim Wachstum:**
* Häufige & uneffiziente, teilweise nicht optimierte / durchdachte **Datenbankabfragen**, die bei vielen Datensätzen zu Performanceproblemen führen könnten. Um das zu vermeiden könnte man **Caching** einführen, Datenbankenabfragen optimieren mit **Indexe**, Joins, usw. Gezielt nachgucken welche Abfragen häufiger sind & wie langsam sie sind.

* **Pagination** bei Transactions fehlt noch. Gerade werden bei `GET /transactions` alle Transaktionen eines Users zurückgegeben. Bei vielen 100en Transaktionen wird das zu viel, gerade müsste Pagination das Frontend übernehmen.

* **Rate limiting** fehlt, macht Brute-Force attacks möglich.

* **Soft deleting** fehlt, wenn sachen gelöscht werden sind sie nicht wiederherstellbar.

* **Zeitzonen & sprache** nicht berücksichtigt, wird problematisch wenn ein französischer Client ein User erstellt & eine Default kategorie mit englischen / deutschen namen erstellt wird.

* Noch kein **logging**, keine Metriken usw. Wenn ein Problem auftritt, ist es schwer zu debuggen, gerade bei Production da Exceptions nur als http status 500 zurückgegeben werden.

## Domain


## Api


## Infra
- postgres Enums instead of varchar for role / type / category_source / standing_order_source