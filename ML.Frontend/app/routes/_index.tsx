import type { MetaFunction } from "@remix-run/node";
import ProtectedRoute from "./components/ProtectedRoute";

export const meta: MetaFunction = () => {
  return [
    { title: "MovieList" },
    { name: "description", content: "Share and manage your movie lists with friends!" },
  ];
};

export default function Index() {
  return (
    <ProtectedRoute>
      <div className="flex h-screen items-center justify-center bg-gray-100 dark:bg-gray-900">
        <div className="flex flex-col items-center gap-8 p-4">
          <header className="flex flex-col items-center gap-4">
            <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100">
              MovieList
            </h1>
            <div className="h-36 w-36">
              <img
                src="/public/logo.png"
                alt="MovieList"
                className="block w-full dark:hidden"
              />
            </div>
          </header>
          <nav className="flex flex-col items-center gap-4 rounded-2xl border border-gray-200 p-6 dark:border-gray-700 bg-white dark:bg-gray-800 shadow-lg">
            <ul className="flex flex-col gap-3">
              {menuItems.map(({ href, text, disabled }) => (
                <li key={href}>
                  {disabled ? (
                    <span
                      className="block text-lg px-6 py-3 rounded-xl text-gray-400 dark:text-gray-600 cursor-not-allowed"
                    >
                      {text}
                    </span>
                  ) : (
                    <a
                      className={`block text-lg px-6 py-3 rounded-xl transition transform hover:scale-105 shadow-md ${text === 'Logout'
                          ? 'text-red-600 hover:bg-red-100 dark:hover:bg-red-900 dark:text-red-400'
                          : 'text-blue-600 hover:bg-blue-100 dark:hover:bg-blue-900 dark:text-blue-400'
                        }`}
                      href={href}
                    >
                      {text}
                    </a>
                  )}
                </li>
              ))}
            </ul>
          </nav>
        </div>
      </div>
    </ProtectedRoute>
  );
}

const menuItems = [
  { href: '/search', text: 'Movie Search' },
  { href: '/lists', text: 'Movie Lists' },
  { href: '/account', text: 'Manage Account', disabled: true },
  { href: '/logout', text: 'Logout' }
];