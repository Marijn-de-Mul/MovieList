# Stage 1: Build the application
FROM node:20 AS builder

WORKDIR /app

# Copy package.json and package-lock.json
COPY package.json package-lock.json ./

# Install dependencies without skipping optional packages
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the application
RUN npm run build

# Stage 2: Setup the production environment
FROM node:20-slim

WORKDIR /app

# Copy built application from the builder stage
COPY --from=builder /app /app

# Expose the application port
EXPOSE 5173

# Start the application
CMD ["npm", "run", "start"]