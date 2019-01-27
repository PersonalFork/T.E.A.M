"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ErrorResponseManager = /** @class */ (function () {
    function ErrorResponseManager() {
    }
    ErrorResponseManager.GetErrorMessageString = function (errorResponse) {
        debugger;
        try {
            var error = errorResponse.error;
            return error["Message"];
        }
        catch (e) {
            return errorResponse.statusText;
        }
    };
    return ErrorResponseManager;
}());
exports.ErrorResponseManager = ErrorResponseManager;
//# sourceMappingURL=ErrorResponseManager.js.map