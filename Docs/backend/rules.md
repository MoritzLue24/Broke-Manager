# Rules

## 1. Allgemeines

## 2. Ownership
Ein User darf nur seine eigenen Daten einsehen & modifizieren lol

## 3. Kategorie-Zurordnung
* Jeder user hat genau eine **default-kategorie**, die nicht bearbeitet oder gelöscht werden kann. Es können keine keywords hinzugefügt werden.

* Wird eine Kategorie **gelöscht**, wird der user gefragt ob er auch alle dazugehörigen Transaktionen löschen lassen möchte. Wenn nicht, werden sie der default-kategorie zugeordnet.

* Nach dem **erstellen** einer neuen **Kategorie** werden alle zutreffenden automatischen änderungen der transaction-kategorie zuordnung zurückgegeben, aber noch nicht angewand. Der user wird gefragt. Treffen mehrere Kategorien auf eine Transaction, werden diese als liste zurückgegeben und der user MUSS eine auswählen.

* Nach dem **erstellen** einer neuen **Transaction** wird bei Treffern gefragt, ob der User der Transaction einer der empfohlenden Kategorien zuordnen möchte. Wenn nicht -> default