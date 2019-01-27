export class ErrorResponseManager {
  static GetErrorMessageString(errorResponse: Response): string {
    debugger
    try {
      var error = errorResponse.error;
      return error["Message"];
    } catch (e) {
      return errorResponse.statusText;
    }
  }
}
