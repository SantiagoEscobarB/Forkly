import { BrowserRouter } from 'react-router';
import SesionProvider from './context/SesionProvider';
import AppRoutes from './routes/AppRoutes';

const App = () => {
    return (
        <SesionProvider>
            <BrowserRouter>
                <AppRoutes />
            </BrowserRouter>
        </SesionProvider>
    );
};

export default App;
