# Medium Clone API Documentation

## Authentication

All protected endpoints require a JWT token in the Authorization header:
```
Authorization: Bearer <token>
```

## Authentication Endpoints

### Register User
- **POST** `/api/users`
- **Body**:
```json
{
    "username": "string",
    "email": "string",
    "password": "string"
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
    "email": "string",
    "password": "string"
}
```
- **Response**: Same as Register

### Get Current User
- **GET** `/api/user`
- **Auth Required**: Yes
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

### Update User
- **PUT** `/api/user`
- **Auth Required**: Yes
- **Body**:
```json
{
    "email": "string",
    "bio": "string",
    "image": "string"
}
```

## Article Endpoints

### Create Article
- **POST** `/api/articles`
- **Auth Required**: Yes
- **Body**:
```json
{
    "title": "string",
    "description": "string",
    "body": "string",
    "tagList": ["string"]
}
```

### Get Articles
- **GET** `/api/articles`
- **Query Parameters**:
  - `limit`: Number of articles (default: 20)
  - `offset`: Offset/skip number (default: 0)
  - `tag`: Filter by tag
- **Response**:
```json
{
    "articles": [
        {
            "slug": "string",
            "title": "string",
            "description": "string",
            "body": "string",
            "tagList": ["string"],
            "createdAt": "datetime",
            "updatedAt": "datetime",
            "favorited": boolean,
            "favoritesCount": number,
            "author": {
                "username": "string",
                "bio": "string",
                "image": "string",
                "following": boolean
            }
        }
    ],
    "articlesCount": number
}
```

### Get Article by Slug
- **GET** `/api/articles/{slug}`
- **Response**: Single article object

### Get Tags
- **GET** `/api/tags`
- **Response**:
```json
{
    "tags": ["string"]
}
```

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
        "following": boolean
    }
}
```

### Follow User
- **POST** `/api/profiles/{username}/follow`
- **Auth Required**: Yes
- **Response**: Profile object

### Unfollow User
- **DELETE** `/api/profiles/{username}/follow`
- **Auth Required**: Yes
- **Response**: Profile object

### Get Following List
- **GET** `/api/profiles/{username}/following`
- **Response**: Array of profiles

### Get Followers List
- **GET** `/api/profiles/{username}/followers`
- **Response**: Array of profiles

## Comment Endpoints

### Add Comment
- **POST** `/api/articles/{slug}/comments`
- **Auth Required**: Yes
- **Body**:
```json
{
    "body": "string"
}
```
- **Response**:
```json
{
    "comment": {
        "id": number,
        "body": "string",
        "createdAt": "datetime",
        "updatedAt": "datetime",
        "author": {
            "username": "string",
            "bio": "string",
            "image": "string"
        }
    }
}
```

### Get Comments
- **GET** `/api/articles/{slug}/comments`
- **Response**: Array of comments

### Delete Comment
- **DELETE** `/api/articles/{slug}/comments/{id}`
- **Auth Required**: Yes

## Common HTTP Status Codes
- 200: Success
- 401: Unauthorized
- 403: Forbidden
- 404: Not Found
- 422: Validation Error