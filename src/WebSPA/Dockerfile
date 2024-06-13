FROM --platform=$BUILDPLATFORM node:22.3.0-bullseye-slim as builder

RUN mkdir /project
WORKDIR /project

RUN npm install -g @angular/cli@17

COPY package.json package-lock.json ./
RUN npm ci

COPY . .
ENTRYPOINT ["ng", "serve"]
CMD ["--host", "0.0.0.0"]
