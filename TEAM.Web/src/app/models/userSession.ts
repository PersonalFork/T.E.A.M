export class UserSession {
  sessionId: string;
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  gender: string;

  constructor(sessionId: string, userId: string, firstName: string, lastName: string) {
    this.sessionId = sessionId;
    this.userId = userId;
    this.firstName = firstName;
    this.lastName = lastName;
  }
}
