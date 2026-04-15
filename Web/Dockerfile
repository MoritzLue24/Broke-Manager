FROM node:25-alpine AS dev
WORKDIR /app

# Dependencies
COPY package*.json ./
RUN npm install

# Other files
COPY . .
EXPOSE 5173

# Run
CMD ["npm", "run", "dev", "--", "--host"]

# TODO: prod