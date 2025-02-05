# Medium Clone API Documentation

## Authentication Endpoints

### Register User
- **POST** `/api/users`
- **Body**: 
```json
{
  "user": {
    "username": "string",
    "email": "string",
    "password": "string"
  }
}
```
- **Response**: 
```json
{
  "user": {
    "email": "string",
    "token": "string",
    "username": "string",
    "bio": "string",
    "image": "string"
  }
}
```

### Login User
- **POST** `/api/users/login`
- **Body**:
```json
{
  "user": {
    "email": "string",
    "password": "string"
  }
}
```
- **Response**: Same as register response

### Get Current User
- **GET** `/api/user`
- **Auth Required**: Yes
- **Headers**: `Authorization: Bearer {token}`
- **Response**: Same as register response

### Update User
- **PUT** `/api/user`
- **Auth Required**: Yes
- **Headers**: `Authorization: Bearer {token}`
- **Body**:
```json
{
  "user": {
    "email": "string",
    "username": "string",
    "password": "string",
    "bio": "string",
    "image": "string"
  }
}
```
- **Response**: Updated user object

## Article Endpoints

### Create Article
- **POST** `/api/articles`
- **Auth Required**: Yes
- **Headers**: `Authorization: Bearer {token}`
- **Body**:
```json
{
  "article": {
    "title": "string",
    "description": "string",
    "body": "string",
    "tagList": ["string"]
  }
}
```
- **Response**:
```json
{
  "article": {
    "slug": "string",
    "title": "string",
    "description": "string",
    "body": "string",
    "tagList": ["string"],
    "createdAt": "datetime",
    "updatedAt": "datetime",
    "favorited": false,
    "favoritesCount": 0,
    "author": {
      "username": "string",
      "bio": "string",
      "image": "string",
      "following": false
    }
  }
}
```

## Comment Endpoints

### Add Comment
- **POST** `/api/articles/{slug}/comments`
- **Auth Required**: Yes
- **Headers**: `Authorization: Bearer {token}`
- **Body**:
```json
{
  "comment": {
    "body": "string"
  }
}
```
- **Response**:
```json
{
  "comment": {
    "id": 1,
    "body": "string",
    "createdAt": "datetime",
    "updatedAt": "datetime",
    "author": {
      "username": "string",
      "bio": "string",
      "image": "string",
      "following": false
    }
  }
}
```

### Get Comments
- **GET** `/api/articles/{slug}/comments`
- **Response**: Array of comment objects

### Delete Comment
- **DELETE** `/api/articles/{slug}/comments/{id}`
- **Auth Required**: Yes
- **Headers**: `Authorization: Bearer {token}`
- **Response**: 200 OK

## Profile Endpoints

### Get Profile
- **GET** `/api/profiles/{username}`
- **Response**:
```json
{
  "profile": {
    "username": "string",
    "bio": "string",
    "image": "string",
    "following": false
  }
}
```

### Follow User
- **POST** `/api/profiles/{username}/follow`
- **Auth Required**: Yes
- **Headers**: `Authorization: Bearer {token}`
- **Response**: Profile object

### Unfollow User
- **DELETE** `/api/profiles/{username}/follow`
- **Auth Required**: Yes
- **Headers**: `Authorization: Bearer {token}`
- **Response**: Profile object

### Get Following List
- **GET** `/api/profiles/{username}/following`
- **Response**: Array of profile objects

### Get Followers List
- **GET** `/api/profiles/{username}/followers`
- **Response**: Array of profile objects

## Error Handling

### Error Response Format
```json
{
  "errors": {
    "body": [
      "error message"
    ]
  }
}
```

Common HTTP Status Codes:
- 200: Success
- 401: Unauthorized
- 403: Forbidden
- 404: Not Found
- 422: Validation Error

## Pagination

Available for article and comment listings:
- `limit`: Number of items to return (default: 20)
- `offset`: Number of items to skip (default: 0)

Example: `/api/articles?limit=20&offset=0`

## Authentication

All protected endpoints require a JWT token in the Authorization header:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

The token is obtained after login/registration and expires after 30 days.