# Tests

## KeywordService

### CreateAsync

**Happy-Path**
Alle Eingaben sind gültig und existieren, es wird ein korrektes ResponseDto zurückgegeben, und das Keyword wurde erstellt.

**Failure-Cases**
Es wird NotFoundException gethrowed, wenn:
* Angegebene Category existiert nicht
* Angegebene Category gehört nicht dem Benutzter
KeywordExistsException, wenn das Keyword bereits existiert (nicht unbedingt nur bei der Category, sondern im ganzen Keywords Table).

### UpdateAsync

**Happy-Path**
Alle Eingaben sind gültig und existieren, es wird nichts zurückgegeben, das Keyword wurde geupdated.

**Failure-Cases**
Es wird NotFoundException gethrowed, wenn:
* Die KeywordId nicht existiert
* Keyword nicht existiert & Category auch nicht
* Die Category nicht den Benutzter gehört
KeywordExistsException, wenn das Keyword bereits existiert (nicht unbedingt nur bei der Category, sondern im ganzen Keywords Table).

### DeleteAllAsync

**Happy-Path**
Die Category gehört dem user, alle Keywords der Category werden gelöscht.

**Failure-Cases**
NotFoundException, wenn:
* Die Category nicht existiert
* Die Category nicht dem user gehört

### DeleteByIdAsync

**Happy-Path**
Die Category gehört dem User, das Keyword gehört der Category. Das Keyword wird gelöscht.

**Failure-Cases**
NotFoundException, wenn:
* Keyword existiert nicht
* Das Keyword nicht zur Category gehört
* DIe Category nicht dem user gehört