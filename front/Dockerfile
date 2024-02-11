FROM node:16 AS build-stage

WORKDIR /app
COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# Step 2: Serve stage
FROM node:16-slim AS serve-stage

RUN npm install -g serve
COPY --from=build-stage /app/dist /app

EXPOSE 5173
CMD ["serve", "-s", "/app", "-l", "5173"]
