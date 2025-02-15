/** @type {import('tailwindcss').Config} */

const getCorrespondingNumber = (index: number): number => {
  if (index===0) {
    return 50
  }

  return index * 100
}

const createMultiColor = (name: string) => {
  let varProperties = {};
  for (let i = 0; i < 10; i++) {
    const varIndex = getCorrespondingNumber(i);
    varProperties = {...varProperties, [varIndex]: (`var(--color-${name}-${varIndex})`)}
  }

  return varProperties;
}

module.exports = {
  content: ["./src/**/*.{html,js,ts}"],
  darkMode: 'selector',
  theme: {
    extend: {
      spacing: {
        "nav-margin": '3.8rem'
      },
      colors: {
        primary: createMultiColor('primary'),
        secondary: createMultiColor('secondary'),
        ternary: createMultiColor('ternary')
      }
    },
  },
  plugins: [],
}

