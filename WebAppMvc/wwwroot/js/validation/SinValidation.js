const sinValidation = () => {
    return {
        validate: value => {
            return {
                valid: validateSIN(value),
                reason: {
                    en: 'The Sin is not valid.',
                    fr: 'The Sin is not valid en francais.',
                },
            };
        },
    };
};

/**
 * Validate a Canadian Social Insurance Number (SIN) using the Luhn algorithm.
 * @param {string} sin - The SIN to validate (can contain spaces or dashes).
 * @returns {boolean} - True if valid, false otherwise.
 */
function validateSIN(sin) {
    if (typeof sin !== "string") {
        return false;
    }

    // Remove spaces and dashes
    const cleaned = sin.replace(/[\s-]/g, "");

    // Must be exactly 9 digits
    if (!/^\d{9}$/.test(cleaned)) {
        return false;
    }

    // Luhn algorithm
    let sum = 0;
    for (let i = 0; i < cleaned.length; i++) {
        let digit = parseInt(cleaned[i], 10);

        // Double every second digit starting from index 1 (second position)
        if (i % 2 === 1) {
            digit *= 2;
            if (digit > 9) digit -= 9;
        }
        sum += digit;
    }

    return sum % 10 === 0;
}
