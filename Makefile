SOLUTION := Broke-Manager.sln

.PHONY: help
help:
	@echo "Usage: make [command]\n"
	@echo "commands:"
	@echo "\tmake clean"

.PHONY: clean
clean:
	@echo "Cleaning all projects"
	dotnet clean $(SOLUTION)
	find . -type d \( -name bin -o -name obj \) -exec rm -rf {} +
